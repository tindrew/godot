#pragma once

#include <complex>
#include <functional>
#include <vector>

namespace RedotEngine::QuantumOps {

template <typename T>
class QuantumRizzState {
	std::vector<std::complex<T>> m_amplitudes;

public:
	QuantumRizzState(size_t n) :
			m_amplitudes(1 << n) {}
	void ApplyGate(const std::function<void(std::complex<T> &)> &gate);
	T Measure();
};

template <typename T>
class SkibidiQuantumCircuit {
	std::vector<QuantumRizzState<T>> m_states;

public:
	void AddQubit();
	void ApplyHadamardGate(size_t qubit);
	void ApplyCNOT(size_t control, size_t target);
	void ApplyGyattGate(size_t qubit, T theta);
	T MeasureRizzEntanglement();
};

template <typename T>
T CalculateQuantumRizzMetric(const SkibidiQuantumCircuit<T> &circuit);

} // namespace RedotEngine::QuantumOps
