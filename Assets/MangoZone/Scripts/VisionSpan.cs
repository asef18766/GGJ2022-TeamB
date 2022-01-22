using Math = System.Math;
using System.Collections.Generic;

using UnityEngine;

namespace MangoZone {

    [RequireComponent(typeof(MeshFilter))]
    public class VisionSpan : MonoBehaviour {

        const int MAX_SEGMENT_LIMIT = 200;
        const int TOTAL_EDGES_RESOLVE_ITERATIONS_LIMIT = 500;
        const float SAME_SIDE_CHECK_DOT_VALUE_TOLERANCE = 0.0003f;

        public static float maxSegmentGapAngle = 1.8f;

        static int currentEdgesResolveIterationsCount = 0;
        static int maxEdgesResolveIterationsCount = 0;

        /// ============================================= ///

        [Header("Properties")]
        public SpanProps spanProps;
        public float     cutInObstacleDistance;
        [SerializeField] Vector3 _localForwardDirection = Vector3.up;
        public Vector3 LocalForwardDirection {
            get {
                if (_localForwardDirection.sqrMagnitude != 1f) {
                    _localForwardDirection = _localForwardDirection.normalized;
                }
                return _localForwardDirection;
            }
        }

        [Header("Statues")]
        public bool isBlind = false;
        public bool isShowingVisionArea = true;
        public bool isFollowingForward = true;

        [Header("Parameters")]
        public LayerMask visionLayerMask;
        public int       edgeResolveIterations;
        public bool      showDebugLines;
        public bool      showVisionEdgeLines;

        [Header("Output Shows")]
        public int segmentCountShows;


        public float FovRateApplied {get; set;} = 1f;
        public Vector3 Origin => transform.position;
        public Vector2 FacingDirection {get; private set;} = Vector2.zero;
        public float CurrentFov => spanProps.fov * FovRateApplied;


        Mesh _mesh;

        void Awake () {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
            FacingDirection = LocalForwardDirection;
        }


        void LateUpdate () {

            if (isFollowingForward) {
                FacingDirection = transform.TransformDirection(LocalForwardDirection);
            }

            if (showVisionEdgeLines && !isBlind) {
                for (int i = -1 ; i <= 1 ; i += 2) {
                    Debug.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(i * CurrentFov / 2, Vector3.forward) * FacingDirection * spanProps.distance, Color.red);
                }
            }


            if (isShowingVisionArea && !isBlind) {
                DrawVisionArea();
            }
            else {
                ClearVisionArea();
            }
        }


        public void SetFacingDirection (Vector2 dir) {
            FacingDirection = dir;
            isFollowingForward = false;
        }

        public bool IsInSight (Vector2 position) {
            if (isBlind)
                return false;

            Vector2 to = position - (Vector2) Origin;
            Vector2 toDir = to.normalized;
            float   toDistance = Vector2.Dot(to, toDir);

            if (IsDirInSightRange(to) && toDistance < spanProps.distance) {

                RaycastHit2D hit = Physics2D.Raycast(Origin, toDir, toDistance, visionLayerMask);

                if (hit.collider == null) {
                    return true;
                }
            }
            return false;
        }

        public bool IsDirInSightRange (Vector2 direction) {
            if (isBlind && direction == Vector2.zero)
                return false;

            if (Vector2.Angle(direction, FacingDirection) < CurrentFov / 2) {
                return true;
            }

            return false;
        }



        void DrawVisionArea () {

            maxEdgesResolveIterationsCount = Math.Max(currentEdgesResolveIterationsCount, maxEdgesResolveIterationsCount);
            // GlobalManager.current.visionSpansMaxEdgesResolveIterationsSoFar = maxEdgesResolveIterationsCount;
            currentEdgesResolveIterationsCount = 0;

            int segmentCount = maxSegmentGapAngle > 0 ? Math.Min( Mathf.CeilToInt(CurrentFov / maxSegmentGapAngle), MAX_SEGMENT_LIMIT ) : 0;
            int rayCount = segmentCount + 1;


            List<ViewCastInfo> casts = new List<ViewCastInfo>();
            ViewCastInfo prevCast = new ViewCastInfo();

            for (int i = 0 ; i < rayCount ; i++) {

                float   angle     = (i - segmentCount / 2f) * CurrentFov / segmentCount;
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.forward) * FacingDirection;

                ViewCastInfo cast = ViewCast(direction);

                if (i > 0 && !IsBothNotCastToObstacle(prevCast, cast) && !IsCastedToSameSideOfObstacle(prevCast, cast)) {

                    List<EdgeInfo> edges = FindEdges(prevCast, cast);

                    foreach (EdgeInfo edge in edges) {
                        casts.Add(edge.viewCastA);
                        casts.Add(edge.viewCastB);
                    }
                }

                if (i == 0 || i == rayCount - 1 || cast.collider == null) {
                    casts.Add(cast);
                }

                prevCast = cast;
            }

            List<Vector3> vertices  = new List<Vector3>();
            List<int>     triangles = new List<int>();

            AddWorldPositionToVerticesList(vertices, Origin);

            int prevHitPointIndex = 0;
            for (int i = 0 ; i < casts.Count ; i++) {
                AddWorldPositionToVerticesList(vertices, casts[i].point);

                int hitPointIndex = vertices.Count - 1;

                // == Debug Draw Lines ==
                if (showDebugLines) {
                    Debug.DrawLine(transform.TransformPoint(vertices[0]), transform.TransformPoint(vertices[prevHitPointIndex]), Color.blue);
                    Debug.DrawLine(transform.TransformPoint(vertices[0]), transform.TransformPoint(vertices[hitPointIndex]), Color.blue);
                }

                if (i > 0) {
                    triangles.Add(0);
                    triangles.Add(hitPointIndex);
                    triangles.Add(prevHitPointIndex);

                    // Add cut in rectangle
                    if (casts[i].collider != null && IsCastedToSameSideOfObstacle(casts[i - 1], casts[i]) && cutInObstacleDistance > Mathf.Epsilon) {

                        AddWorldPositionToVerticesList(vertices, casts[i - 1].point - casts[i - 1].normal * cutInObstacleDistance);
                        AddWorldPositionToVerticesList(vertices, casts[i].point - casts[i].normal * cutInObstacleDistance);

                        triangles.Add(prevHitPointIndex);
                        triangles.Add(vertices.Count - 1);
                        triangles.Add(vertices.Count - 2);

                        triangles.Add(prevHitPointIndex);
                        triangles.Add(hitPointIndex);
                        triangles.Add(vertices.Count - 1);
                    }
                }

                prevHitPointIndex = hitPointIndex;
            }


            _mesh.Clear();
            _mesh.vertices  = vertices.ToArray();
            _mesh.triangles = triangles.ToArray();


            segmentCountShows = segmentCount;
        }

        void ClearVisionArea () {
            _mesh.Clear();
        }



        ViewCastInfo ViewCast (Vector3 direction) {
            RaycastHit2D hit = Physics2D.Raycast(Origin, direction, spanProps.distance, visionLayerMask);

            Vector3 hitPos = new Vector3(hit.point.x, hit.point.y, Origin.z);

            if (hit.collider != null)
                return new ViewCastInfo(direction, hit.collider, hit.distance, hitPos, hit.normal);
            else
                return new ViewCastInfo(direction, hit.collider, spanProps.distance, Origin + direction * spanProps.distance, Vector2.zero);
        }

        List<EdgeInfo> FindEdges (ViewCastInfo castA, ViewCastInfo castB) {

            List<EdgeInfo> result = new List<EdgeInfo>();

            for (int j = 0 ; j < edgeResolveIterations ; j++) {
                if (currentEdgesResolveIterationsCount >= TOTAL_EDGES_RESOLVE_ITERATIONS_LIMIT)
                    break;

                currentEdgesResolveIterationsCount++;

                ViewCastInfo cast = ViewCast( (castA.direction + castB.direction).normalized );

                if (IsBothNotCastToObstacle(cast, castA) || IsCastedToSameSideOfObstacle(cast, castA)) {
                    castA = cast;
                }
                else if (IsBothNotCastToObstacle(cast, castB) || IsCastedToSameSideOfObstacle(cast, castB)) {
                    castB = cast;
                }
                else {
                    result.AddRange( FindEdges(castA, cast) );
                    result.AddRange( FindEdges(cast, castB) );
                    return result;
                }
            }

            result.Add(new EdgeInfo(castA, castB));
            return result;
        }


        bool IsBothNotCastToObstacle (ViewCastInfo castA, ViewCastInfo castB) {
            return castA.collider == null && castB.collider == null;
        }

        bool IsCastedToSameSideOfObstacle (ViewCastInfo castA, ViewCastInfo castB) {

            if (castA.normal == castB.normal && Mathf.Abs(Vector2.Dot(castB.point - castA.point, castA.normal)) < SAME_SIDE_CHECK_DOT_VALUE_TOLERANCE) {

                if (castA.collider == castB.collider || castA.collider.Distance(castB.collider).distance <= 0) {
                    return true;
                }
            }
            return false;
        }


        void AddWorldPositionToVerticesList (List<Vector3> vertices, Vector3 worldPos) {
            vertices.Add(transform.InverseTransformPoint(worldPos));
        }


        [System.Serializable]
        public struct SpanProps {
            public float distance;
            public float fov;

            public SpanProps (float distance, float fov) {
                this.distance = distance;
                this.fov = fov;
            }
        }

        struct ViewCastInfo {
            // public Vector2    origin;
            public Vector3    direction;
            public Collider2D collider;
            public float      distance;
            public Vector3    point;
            public Vector3    normal;

            public ViewCastInfo (Vector3 dir, Collider2D col, float dist, Vector3 p, Vector3 norm) {
                // origin = org;
                direction = dir;
                collider = col;
                distance = dist;
                point = p;
                normal = norm;
            }
        }

        struct EdgeInfo {
            public ViewCastInfo viewCastA;
            public ViewCastInfo viewCastB;

            public EdgeInfo (ViewCastInfo castA, ViewCastInfo castB) {
                viewCastA = castA;
                viewCastB = castB;
            }
        }
    }

}